import { createRouter, createWebHashHistory} from "vue-router";
import HomePage from "@/pages/HomePage.vue"
import TripPage from "@/pages/TripPage.vue"
import LoginPage from "@/pages/LoginPage.vue"
import AdminPage from "@/pages/AdminPage.vue"

import { store } from '@/store'

const routes = [
    {
      path: '/',
      name: 'home',
      component: HomePage,
      meta: {
        title: "Pinsetur"
      }
    },
    {
      path: '/trips/:id',
      meta: {
        title: "Pinsetur"
      },
      component: TripPage,
      props: true
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
      meta: {
        title: "Pinsetur"
      }
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminPage,
      meta: {
        title: "Pinsetur"
      }
    }
  ]

const router = createRouter({
    history: createWebHashHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
      // always scroll to top
      return { top: 0 }
    }
});

router.beforeEach(async (to) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login'];
  const authRequired = !publicPages.includes(to.path);

  if (authRequired && !store.getters.isAuthenticated) {
      return { name: 'login'}
  }

  // Redirect to main if already logged in
  if (store.getters.isAuthenticated && to.path === '/login') {
    return { name: 'home'}
  }
})

export default router