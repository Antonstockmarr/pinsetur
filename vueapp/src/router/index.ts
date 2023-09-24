import { createRouter, createWebHashHistory} from "vue-router";
import HomePage from "@/pages/HomePage.vue"
import TripPage from "@/pages/TripPage.vue"
import LoginPage from "@/pages/LoginPage.vue"

import { store } from '@/store'

const routes = [
    {
      path: '/',
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
      component: LoginPage,
      meta: {
        title: "Pinsetur"
      }
    }
  ]

const router = createRouter({
    history: createWebHashHistory(),
    routes, // short for `routes: routes`
});

router.beforeEach(async (to) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login'];
  const authRequired = !publicPages.includes(to.path);

  if (authRequired && !store.getters.isAuthenticated) {
      return '/login';
  }
})

export default router