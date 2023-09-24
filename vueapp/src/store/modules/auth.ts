import { User } from '@/Models/User';
import { State } from '..'

const state = {
    user: null,
    token: null
};
const getters = {
    isAuthenticated: (state: State) => !!state.user?.token,    
    StateUser: (state: State) => state.user,
};
const actions = {
};
const mutations = {
    setUser(state: State, user: User){
        state.user = user
    },
    LogOut(state: State){
        state.user = null
    },
};
export default {
  state,
  getters,
  actions,
  mutations
};
