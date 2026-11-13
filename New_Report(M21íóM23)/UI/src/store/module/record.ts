import { defineStore } from 'pinia';
import { store } from '@/store';
interface Record {
    Answer: Array<any>,
    Subject: Array<any>,
    isComplete: Boolean,
    Record:Number,
}

export const AnswerRecord = defineStore({
    id: 'AnswerRecord',
    state: (): Record => ({
        Answer: [],
        Subject: [],
        isComplete: false,
        Record:0
    }),
    actions: {
        AddAnswer(answer: any) {
            // if (this.Answer.length > 0) {
            //     for (let i in this.Answer) {
            //         if (this.Answer[i]['id'] == answer['id']) {
            //             this.Answer[i]['useranswer'] = answer['useranswer']
            //         } else {

            //         }
            //     }
            // } else {
            //     this.Answer.push(answer)
            // }
            const isRepeat = this.Subject.indexOf(answer.id)
            if (isRepeat == -1) {
                this.Subject.push(answer.id);//为什么使用两个数组?因为按照上面一个数组的做法我找不到不重复添加元素且对重复元素进行修改的方法
                this.Answer.push(answer)//多一个数组记录id后很轻松就做到了而且更简洁
            } else {
                this.Answer[isRepeat]['useranswer'] = answer['useranswer']
            }

        },
        ClearAnswer() {
            this.Answer = []
        },
        ChangeCompleteStatus(status: Boolean) {
            this.isComplete = status
        },
        ChangeRecord(params:Number){
            this.Record = params;
        }
    },
})

export function AnswerRecordWithOut() {
    return AnswerRecord(store);
}