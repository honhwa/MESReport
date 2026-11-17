const parser = require('@babel/parser');
const loaderUtils = require('loader-utils');
const total = {
    len: 0,
    components: {}
};
// 对象排序
const sortable = (obj) => Object.fromEntries(Object.entries(obj).sort(([, a], [, b]) => b - a));
module.exports = function (source) {
    const Regex1 = /import\s*{\s*([\w\s,]+)\s*}\s*from\s*['"]@\/components\/core\/.*?['"]/g;
    // const Regex1 = /import\s*{\s*([\w\s,]+)\s*}\s*from\s*['"]@\/components\/core\/dynamic-table['"]\s*/g;
    // let source1 = "";
    const matchResult1 = source.match(Regex1);
    const callback = this.async();

    try {
        if (matchResult1.length > 0) {
            setTimeout(() => {
                for (let i in matchResult1) {
                    let source1 = Regex1.exec(matchResult1[i]);
                    if (source1&&source1.length > 0) {
                        let name = source1[1];
                        total.len++;
                        total.components[name] = total.components[name] ? total.components[name] + 1 : 1;
                        total.components = sortable(total.components);
                    }
                }
                console.log(total, 'total');

                callback(null, source);
            }, 0);
        } else callback(null, source);
    } catch (error) {
        // console.log("error"+error);
        callback(null, source);
    }
};
