import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { store, key } from './store'
import { BootstrapVueNext } from 'bootstrap-vue-next'
import VueGoogleMaps from '@fawmi/vue-google-maps'
import { $api } from './common/apiService'

// Import Bootstrap and BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-next/dist/bootstrap-vue-next.css'
import '@/reset.css'

async function initApp() {
    // Attempt silent refresh before the router evaluates any guards.
    // If a valid refresh token cookie exists the user is restored without seeing the login page.
    try {
        const session = await $api.auth.refresh()
        if (session) {
            await store.dispatch('setSession', session)
            const sasToken = await $api.token.get()
            if (sasToken) await store.dispatch('setSasToken', sasToken)
        }
    } catch {
        // Refresh failed — router guard will redirect to /login
    }

    createApp(App)
        .use(router)
        .use(BootstrapVueNext)
        .use(VueGoogleMaps, {
            load: {
                key: process.env.VUE_APP_GOOGLE_API_KEY
            }
        })
        .use(store, key)
        .mount('#app')
}

initApp()
