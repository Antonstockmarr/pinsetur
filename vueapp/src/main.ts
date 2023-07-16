import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { BootstrapVueNext } from 'bootstrap-vue-next'
import VueGoogleMaps from '@fawmi/vue-google-maps'

// Import Bootstrap and BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-next/dist/bootstrap-vue-next.css'


createApp(App)
    .use(router)
    .use(BootstrapVueNext)
    .use(VueGoogleMaps, {
        load: {
            key: process.env.VUE_APP_GOOGLE_API_KEY
        }
    })
    .mount('#app')
