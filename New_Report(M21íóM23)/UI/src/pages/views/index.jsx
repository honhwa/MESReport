import { defineComponent } from 'vue';
import {  useRoute } from 'vue-router';
import { permmenu, } from '@/api/system/menu';
import { Storage } from '@/utils/Storage';
import { watch, ref } from 'vue';
import { useUserStore } from '@/store/module/user';


export default defineComponent({
    name: 'RouterPage',
    setup() {
        const router = useRoute();
        let useUserParams = useUserStore();
        let roData = ref([]);
        let dawi = ref([]);
        const loadTableData = async (params) => {
            let Data = useUserParams.routerPage;
            getRouter(Data, params);
        };
        function getRouter(Data, params) {
            Data.forEach((item) => {
                if (item.path != params && item.children) {
                    getRouter(item.children, params);
                } else {
                    if (item.path == params) {
                        roData.value = item;
                    }
                }
            });
        }
       
        watch(
            () => router?.fullPath,
            async (newValue) => {
                await loadTableData('/FSUI/LCM1/OQC');
                console.log(roData);
                dawi.value = roData.value.children.map(item => (
                    <>
                    <ul>
                        <li><RouterLink to={item.router}>{item.meta.title }</RouterLink></li>
                    </ul>
                    </>
                ))
            },
            {
                immediate: true,//立即執行一次
                deep: true
            })
        return () => (
            <div>
               <div>{dawi.value}</div>
            </div>
        );
    },
});
