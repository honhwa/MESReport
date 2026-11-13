import { unref, toRaw, ref } from 'vue';
import { cloneDeep, uniqBy } from 'lodash-es';
import dayjs from 'dayjs';
import { dateItemType, handleInputNumberValue } from '../helper';
import type { FormSchema } from '../types/form';
import type { NamePath } from 'ant-design-vue/lib/form/interface';
import type { FormState, FormMethods } from './index';
import type { SchemaFormEmitFn } from '../schema-form';
import { isArray, isFunction, isObject, isString } from '@/utils/is';
import { deepMerge } from '@/utils/index';
import { GetCommon } from '@/api/pages/common';
import { Storage } from '@/utils/Storage';
import router from '@/router';

type UseFormActionContext = FormState & {
  emit: SchemaFormEmitFn;
  handleFormValues: FormMethods['handleFormValues'];
};

export type FormEvents = ReturnType<typeof useFormEvents>;

export function useFormEvents(formActionContext: UseFormActionContext) {
  const {
    emit,
    formPropsRef,
    formSchemasRef,
    formModel,
    cacheFormModel,
    getFormProps,
    schemaFormRef,
    defaultFormValues,
    handleFormValues,
  } = formActionContext;

  function getFieldsValue(): Recordable {
    const formEl = unref(schemaFormRef);
    if (!formEl) return {};
    return handleFormValues(toRaw(unref(formModel)));
  }

  /**
   * @description: Is it time
   */
  function itemIsDateType(key: string) {
    return unref(formPropsRef).schemas?.some((item) => {
      return item.field === key && isString(item.component)
        ? dateItemType.includes(item.component)
        : false;
    });
  }

  /**
   * @description: 设置表单字段值
   */
  async function setFieldsValue(values: Recordable): Promise<void> {
    const schemas = unref(formSchemasRef);
    const fields = schemas.map((item) => item.field).filter(Boolean);

    Object.assign(cacheFormModel, values);

    const validKeys: string[] = [];
    Object.keys(values).forEach((key) => {
      const schema = schemas.find((item) => item.field === key);
      let value = values[key];

      const hasKey = Reflect.has(values, key);
      if (isString(schema?.component)) {
        value = handleInputNumberValue(schema?.component, value);
      }
      // 0| '' is allow
      if (hasKey && fields.includes(key)) {
        // time type
        if (itemIsDateType(key)) {
          if (Array.isArray(value)) {
            const arr: any[] = [];
            for (const ele of value) {
              arr.push(ele ? dayjs(ele) : null);
            }
            formModel[key] = arr;
          } else {
            const { componentProps } = schema || {};
            let _props = componentProps as any;
            if (isFunction(componentProps)) {
              _props = _props({ formPropsRef, formModel });
            }
            formModel[key] = value ? (_props?.valueFormat ? value : dayjs(value)) : null;
          }
        } else {
          formModel[key] = value;
        }
        validKeys.push(key);
      }
    });
    validateFields(validKeys);
  }

  async function resetSchema(data: Partial<FormSchema> | Partial<FormSchema>[]) {
    let updateData: Partial<FormSchema>[] = [];
    if (isObject(data)) {
      updateData.push(data as FormSchema);
    }
    if (isArray(data)) {
      updateData = [...data];
    }
    //@ts-ignore
    unref(formPropsRef).schemas = updateData as FormSchema[];
  }

  /**
   * @description: 插入到指定 filed 后面，如果没传指定 field，则插入到最后,当 first = true 时插入到第一个位置
   * @param schemaItem要插入的表单项
   * @param prefixField 指定filed之后插入
   */
  async function appendSchemaByField(schemaItem: FormSchema, prefixField?: string, first = false) {
    //@ts-ignore
    const schemaList = cloneDeep<FormSchema[]>(unref(formSchemasRef));
    // 插入的索引
    const index = schemaList.findIndex((schema) => schema.field === prefixField);
    if (Array.isArray(schemaItem) && schemaItem.length) {
      // 保存上一次插入的位置
      let lastInsertLocation = prefixField;
      let lastInsertindex = schemaList.findIndex((schema) => schema.field === lastInsertLocation);
      for (let i = 0; i < schemaItem.length; i++) {
        // 插入的索引
        // 如果prefixField存在，同時找不到，同時first為true
        if (!prefixField || lastInsertindex === -1 || first) {
          first ? schemaList.unshift(schemaItem[i]) : schemaList.push(schemaItem[i]);
          formModel[schemaItem[i].field] = schemaItem[i].defaultValue;
          formPropsRef.value.schemas = schemaList;
          return;
        }
        // 如果找到，直接插入
        if (lastInsertindex !== -1) {
          schemaList.splice(lastInsertindex + 1, 0, schemaItem[i]);
        }
        formModel[schemaItem[i].field] = schemaItem[i].defaultValue;
        formPropsRef.value.schemas = schemaList;
        // 設置為當前的filed
        lastInsertindex = schemaList.findIndex((schema) => schema.field === schemaItem[i].field);
      }
    } else {
      if (!prefixField || index === -1 || first) {
        first ? schemaList.unshift(schemaItem) : schemaList.push(schemaItem);
        formModel[schemaItem.field] = schemaItem.defaultValue;
        formPropsRef.value.schemas = schemaList;
        return;
      }
      if (index !== -1) {
        schemaList.splice(index + 1, 0, schemaItem);
      }
      formModel[schemaItem.field] = schemaItem.defaultValue;
      formPropsRef.value.schemas = schemaList;
    }
  }

  /**
   * @description: 根据 field 删除 Schema
   */
  async function removeSchemaByFiled(fields: string | string[]): Promise<void> {
    //@ts-ignore
    const schemaList = cloneDeep<FormSchema[]>(unref(formSchemasRef));

    if (!fields) {
      return;
    }

    let fieldList: string[] = isString(fields) ? [fields] : fields;
    if (isString(fields)) {
      fieldList = [fields];
    }
    for (const field of fieldList) {
      if (isString(field)) {
        const index = schemaList.findIndex((schema) => schema.field === field);
        if (index !== -1) {
          Reflect.deleteProperty(formModel, field);
          schemaList.splice(index, 1);
        }
      }
    }
    formPropsRef.value.schemas = schemaList;
  }

  /**
   * @description  更新formItemSchema
   */
  async function updateSchema(data: Partial<FormSchema> | Partial<FormSchema>[]) {
    let updateData: Partial<FormSchema>[] = [];
    if (isObject(data)) {
      updateData.push(data as FormSchema);
    }
    if (isArray(data)) {
      updateData = [...data];
    }
    const hasField = updateData.every(
      (item) => item.component === 'Divider' || (Reflect.has(item, 'field') && item.field),
    );

    if (!hasField) {
      console.error(
        'All children of the form Schema array that need to be updated must contain the `field` field',
      );
      return;
    }
    const schemas: FormSchema[] = [];
    updateData.forEach((item) => {
      unref(formSchemasRef).forEach((val) => {
        if (val.field === item.field) {
          const newSchema = deepMerge(val, item);

          schemas.push(newSchema);
        } else {
          schemas.push(val);
        }
      });
    });

    unref(formPropsRef).schemas = uniqBy(schemas, 'field');
  }

  async function resetFields(): Promise<void> {
    const { resetFunc, submitOnReset } = unref(getFormProps);
    resetFunc && isFunction(resetFunc) && (await resetFunc());

    Object.keys(formModel).forEach((key) => {
      formModel[key] = defaultFormValues[key];
    });

    emit('reset', formModel);
    submitOnReset && handleSubmit();
    setTimeout(clearValidate);
  }

  async function validateFields(nameList?: NamePath[] | undefined) {
    return schemaFormRef.value?.validateFields(nameList);
  }

  async function validate(nameList?: NamePath[] | undefined) {
    return await schemaFormRef.value?.validate(nameList)!;
  }

  async function clearValidate(name?: string | string[]) {
    await schemaFormRef.value?.clearValidate(name);
  }

  async function scrollToField(name: NamePath, options?: ScrollOptions | undefined) {
    await schemaFormRef.value?.scrollToField(name, options);
  }

  /**
   * @description  添加点击率
   */
  async function InsertClick() {
    let item = router.currentRoute.value;
    let ClickMoudle = item.path.split('/');
    let UserMessage = Storage.get('USERMESSAGE')!;
    let params = {
      apiName: `${process.env.VUE_APP_SITE}AP_Number_Of_Hits_Api`,
      SysName: `${process.env.VUE_APP_SITE}Report`,
      SysModule: ClickMoudle[2],
      SysFunc: item?.meta.title,
      EmpName: UserMessage.MemName,
      EmpNo: UserMessage.MemID,
      requestip: UserMessage.RemoteIP,
      MODULE: ClickMoudle[3],
      REPORT_ID: item?.name,
    };
    await GetCommon(params);
  }

  async function handleSubmit(e?: Event): Promise<void> {
    e && e.preventDefault();
    const { submitFunc } = unref(getFormProps);
    if (submitFunc && isFunction(submitFunc)) {
      await submitFunc();
      return;
    }
    const formEl = unref(schemaFormRef);
    if (!formEl) return;
    try {
      const values = await validate();
      await InsertClick();
      const res = handleFormValues(values);
      emit('submit', res);
    } catch (error: any) {
      return Promise.reject(error);
    }
  }
  async function upload(e?: Event): Promise<void> {
    e && e.preventDefault();
    const { submitFunc } = unref(getFormProps);
    if (submitFunc && isFunction(submitFunc)) {
      await submitFunc();
      return;
    }
    const formEl = unref(schemaFormRef);
    if (!formEl) return;
    try {
      const values = await validate();
      await InsertClick();
      const res = handleFormValues(values);
      emit('upload', res);
    } catch (error: any) {
      return Promise.reject(error);
    }
  }

  async function custom(e?: Event): Promise<void> {
    e && e.preventDefault();
    const { submitFunc } = unref(getFormProps);
    if (submitFunc && isFunction(submitFunc)) {
      await submitFunc();
      return;
    }
    const formEl = unref(schemaFormRef);
    if (!formEl) return;
    try {
      const values = await validate();
      await InsertClick();
      const res = handleFormValues(values);
      emit('custom', res);
    } catch (error: any) {
      return Promise.reject(error);
    }
  }

  return {
    submit: handleSubmit,
    upload,
    clearValidate,
    validate,
    validateFields,
    getFieldsValue,
    updateSchema,
    resetSchema,
    appendSchemaByField,
    removeSchemaByFiled,
    resetFields,
    setFieldsValue,
    scrollToField,
    custom,
  };
}
