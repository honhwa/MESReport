import { defineStore } from 'pinia';
import { store } from '@/store';
interface Judge {
    Standard: Object
}

export const StandardAnswer = defineStore({
    id: 'StandardAnswer',
    state: (): Judge => ({
        Standard: {}
    }),
    actions: {
        AddAnswer(Answer: Object) {
            this.Standard = Answer
        },
        ClearAnswer() {
            this.Standard = {}
        },

    },
})

export function StandardAnswerWithOut() {
    return StandardAnswer(store);
}