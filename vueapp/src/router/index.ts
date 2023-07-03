import { createRouter, createWebHashHistory} from "vue-router";
import HomePage from "@/pages/HomePage.vue"
import TripPage from "@/pages/TripPage.vue"

const routes = [
    { path: '/', component: HomePage },
    { path: '/trips/:id', component: TripPage, props: true}
  ]

export default createRouter({
    history: createWebHashHistory(),
    routes, // short for `routes: routes`
});