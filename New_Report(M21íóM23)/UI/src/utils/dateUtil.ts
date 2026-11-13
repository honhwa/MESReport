/**
 * Independent time operation tool to facilitate subsequent switch to dayjs
 */
import { useLocale } from '@/locales/useLocale';
import dayjs from 'dayjs';
import { watchEffect, unref } from 'vue';
require('dayjs/locale/zh-cn')



const { getLocale } = useLocale();

watchEffect(() => {
  let Locale = unref(getLocale)== "zh_CN" ? "zh-cn" : "en"
  dayjs.locale(Locale)
});
const DATE_TIME_FORMAT = 'YYYY-MM-DD HH:mm:ss';
const DATE_FORMAT = 'YYYY-MM-DD';
const DATE_HOUR_FORMAT = 'YYYY-MM-DD HH:mm'

export function formatToDateTime(
  date: dayjs.Dayjs | undefined = undefined,
  format = DATE_TIME_FORMAT,
): string {
  let time = dayjs(date).format(format) == 'Invalid Date' ? '无' : dayjs(date).format(format)
  return time;
}

export function formatToDateHour(
  date: dayjs.Dayjs | undefined = undefined,
  format = DATE_FORMAT,
): string {
  let time = `${dayjs(date).format(format)} 07:30`
  let defeaulttime = dayjs(time).format(DATE_HOUR_FORMAT)
  // console.log(defeaulttime);

  return defeaulttime
}


export function formatToDate(
  // date: dayjs.Dayjs | undefined = undefined,
  format = DATE_FORMAT,
): string {
  return dayjs(new Date().getTime()).format(format);
}
// 前一天日期
export function PreDate(format = DATE_FORMAT): string {
  let date = (new Date().getTime()) - 86400000;
  return dayjs(date).format(format);
}
export const dateUtil = dayjs;
