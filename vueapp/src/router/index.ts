import { createRouter, createWebHashHistory} from "vue-router";
import HomePage from "@/pages/HomePage.vue"

const routes = [
    { path: '/', component: HomePage }
  ]

export default createRouter({
    history: createWebHashHistory(),
    routes, // short for `routes: routes`
});