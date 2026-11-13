import { defineStore } from 'pinia';
import { store } from '@/store';
interface CollapseStatus {
    collapse: Boolean
}

export const sidebarcollsapse = defineStore({
    id: 'side',
    state: (): CollapseStatus => ({
        collapse: false
    }),
    actions: {
        changestatus() {
            this.collapse = !this.collapse
        }
    },
})

export function sidebarcollsapseWithOut() {
    return sidebarcollsapse(store);
  }