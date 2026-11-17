import { DefineComponent,Ref } from 'vue';
import VexTable from './src/vex-virtual-table.vue';
import type { VexVirtualTableProps,VexTableInstance } from './src/vex-virtual-table';


// export * from './src/types/';
export * from './src/hooks/';
export * from './src/vex-virtual-table';

export { VexTable };

export type VexcTableRef = Ref<VexTableInstance>;

export default VexTable as unknown as DefineComponent<Partial<VexVirtualTableProps>>;