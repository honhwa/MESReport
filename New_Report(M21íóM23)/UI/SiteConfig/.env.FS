# 只在正式环境中被载入
ENV = 'production'

# 网站前缀
BASE_URL = /

# base api
# VUE_APP_BASE_API = 'http://10.189.127.62:22000'.
VUE_APP_BASE_API = 'http://fsreportapi.cminl.oa/'
VUE_APP_BASE_SOCKET_PATH = '/ws-api'
VUE_APP_BASE_SOCKET_NSP = '/admin'

# mock api
VUE_APP_MOCK_API = '/mock-api/'
// VUE_APP_VERIFYTOKEN= 'http://TSAMv4Athe.cminl.oa'
VUE_APP_VERIFYTOKEN= 'http://psamv4athefs.cminl.oa'
VUE_APP_SAM = 'http://tsamv4atho.cminl.oa'
VUE_APP_SYSID='FSUI'
VUE_APP_SITE='FS'