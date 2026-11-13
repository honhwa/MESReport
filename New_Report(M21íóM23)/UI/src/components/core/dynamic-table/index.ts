import DynamicTable from './src/dynamic-table.vue';
import type { DefineComponent, Ref } from 'vue';
import type { DynamicTableInstance, DynamicTableProps } from './src/dynamic-table';
export { DynamicTable };

export * from './src/types/';
export * from './src/hooks/';
export * from './src/dynamic-table';

export type DynamicTableRef = Ref<DynamicTableInstance>;

export default DynamicTable as unknown as DefineComponent<Partial<DynamicTableProps>>;
