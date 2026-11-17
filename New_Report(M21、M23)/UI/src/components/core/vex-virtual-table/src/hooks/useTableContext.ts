import { provide, inject } from 'vue';
import type { VexTableType } from './index';

const key = Symbol('vex-table');

export function createTableContext(instance: VexTableType) {
  provide(key, instance);
}

export function useTableContext() {
  return inject(key) as VexTableType;
}
