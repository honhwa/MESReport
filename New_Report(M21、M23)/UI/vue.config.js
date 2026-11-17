const path = require('path');
const resolve = (dir) => path.join(__dirname, dir); // 路径
const pkg = require('./package.json');
// const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

process.env.VUE_APP_VERSION = pkg.version;

const Timestamp = new Date().getTime();

const IS_PROD = ['production', 'prod'].includes(process.env.NODE_ENV);
const IS_DEV = ['development'].includes(process.env.NODE_ENV);

// port = 8098 npm run dev OR npm run dev --port = 8098
const port = process.env.port || process.env.npm_config_port || 8080; // dev port

let cdn = {
    css: [
        'https://cdn.bootcdn.net/ajax/libs/ant-design-vue/3.2.10/antd.min.css',
        'https://cdn.bootcdn.net/ajax/libs/vxe-table/4.3.11/style.css'
    ],
    js: []
};
let externals = {
};

// if (IS_PROD) {
//     externals = {
//         vue: 'Vue',
//         tinymce: 'Tinymce',
//         xlsx: 'XLSX',
//         'ant-design-vue': 'ant-design-vue',
//         exceljs: 'exceljs',
//         echarts: 'echarts',
//         'vxe-table': 'vxe-table'
//     };
//     cdn.js = [
//         'http://fsreportapi.cminl.oa/api/Interface/DownloadFile?DownloadTicket=vue.js',
//         'https://cdn.bootcdn.net/ajax/libs/ant-design-vue/3.2.10/antd.min.js',
//         'http://fsreportapi.cminl.oa/api/Interface/DownloadFile?DownloadTicket=tinymce.min.js',
//         'https://cdn.bootcdn.net/ajax/libs/xlsx/0.18.5/xlsx.full.min.js',
//         'https://cdn.bootcdn.net/ajax/libs/exceljs/4.3.0/exceljs.min.js',
//         'https://cdn.bootcdn.net/ajax/libs/echarts/5.3.3/echarts.min.js',
//         'https://cdn.bootcdn.net/ajax/libs/vxe-table/4.3.11/vxe-table.min.js'
//     ];
// }

module.exports = {
    devServer: {
        host: '0.0.0.0',
        port: port,
        open: true,
        overlay: {
            warnings: false,
            errors: true
        },
        https: false
        /* proxy: {//配置跨域
            '/api': {
              target: 'http://10.189.127.62:22000',//这里后台的地址模拟的;这里最好使用公共地址
              ws: true,
              changeOrigin: true,//允许跨域
              pathRewrite: {
                '^/api': ''//请求的时候使用这个api就可以写公共地址后的地址
              },
              logLevel: 'debug' // 用于检查代理的真实地址
            }
          } */
    },
    css: {
        sourceMap: !IS_PROD,
        loaderOptions: {
            less: {
                javascriptEnabled: true
            }
        }
    },
    configureWebpack: (config) => {
        if(IS_PROD){
        // webpack 配置
        config.output.filename = `static/js/[name].${process.env.VUE_APP_Version}.${Timestamp}.js`;
        config.output.chunkFilename = `static/js/[name].${process.env.VUE_APP_Version}.${Timestamp}.js`;
        // config.module.rules.push({
        //     test: /\.(vue|)$/,
        //     exclude: /node_modules/,
        //     use: [
        //         {
        //             loader: path.resolve(__dirname, './loaderCount.js'),
        //             options: {
        //                 packageName: '@/components/core'
        //             }
        //         }
        //     ]
        // });
        config.externals = externals;
    }
        // config.plugins = [
        //     // new HappyPack({
        //     //     id: 'happybabel',
        //     //     loaders: ['babel-loader'],
        //     //     threadPool: happyThreadPool
        //     // }),
        //     new UglifyJsPlugin({
        //         uglifyOptions: {
        //             warnings: false,
        //             compress: {
        //               // 移除 debugger
        //               drop_debugger: true,
        //               // 移除console.*函数
        //               drop_console: true,
        //               // 移除console.log的引用
        //               // 例如 log = console.log, 移除log，同时移除console.log
        //               pure_funcs: ['console.log']
        //             }
        //           },
        //           // 多进程并行运行
        //           parallel: true,
        //           // 启用缓存
        //           cache: true,
        //           // 抽取注释
        //           extractComments: true

        //       }),
        // ];
    },
    chainWebpack: (config) => {
        config.optimization.minimizer('terser').tap((args) => {
            args[0].terserOptions.compress.drop_console = true;
            return args;
        });
        config.plugin('html').tap((args) => {
            args[0].cdn = cdn;
            return args;
        });
        // 讓 VUE_APP_BASE_API 在發佈後可透過 dist/config.js (window.__APP_CONFIG__) 覆寫，無需重新建置
        config.plugin('define').tap((args) => {
            args.forEach((arg) => {
                if (arg && arg['process.env']) {
                    arg['process.env'].VUE_APP_BASE_API =
                        '(window.__APP_CONFIG__ && window.__APP_CONFIG__.VUE_APP_BASE_API) || ' +
                        JSON.stringify(process.env.VUE_APP_BASE_API);
                }
            });
            return args;
        });
    },
    publicPath: './',
    assetsDir: 'static'

    // configureWebpack: (config) => {
    //     // 开启顶级await
    //     config.experiments = {
    //         topLevelAwait: true
    //     };
    // },
    // chainwebpack: (config) => {//搞不懂这也报错T.T
    //     config.module.rule('css').exclude.add(resolve('node_modules/ant-design-vue/dist/antd.dark.css')).end();
    //     config.module.rule('raw-css').resourceQuery(/raw/).type('asset/source');
    // },
    /*  devServer: {
         host: '0.0.0.0',
         port: port,
         open: true,
         overlay: {
             warnings: false,
             errors: true
         },
         proxy: {
             '/MasterFlowAPI': {
                 target: 'http://TJNFLOW.CMINL.OA',
                 changeOrigin: true
             },
             // '^/api': {
             //     // target: process.env.VUE_APP_API_URL,
             //     target: 'https://nest-api.buqiyuan.site/api/',
             //     // target: 'http://localhost:7001 ',
             //     changeOrigin: true,
             //     logLevel: 'debug',
             //     pathRewrite: {
             //         '^/api': ''
             //     }
             // }
         }
     } */
};
