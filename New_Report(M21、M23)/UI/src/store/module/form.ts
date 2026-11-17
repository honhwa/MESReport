import { defineStore } from 'pinia';
import { store } from '@/store';
interface Record {
    isAddForm: Boolean,
    MoveCount:number
}

export const FormStatus = defineStore({
    id: 'FormStatus',
    state: (): Record => ({
        isAddForm: false,
        MoveCount:1
    }),
    actions: {
        changeFormStatus(status: boolean) {
            this.isAddForm = status
        },
        AddMoveCount(){
            this.MoveCount+=1
        },
        ChangeMoveCount(){
            this.MoveCount=1
        }
    },
})

export function FormStatusWithOut() {
    return FormStatus(store);
}