import { User } from '@/Models/User';

export type State = {
    user: User | null
    token: string | null
}
const state: State = {
    user: { jwt: "", username: ""},
    token: null
};
const getters = {
    isAuthenticated: (state: State) => !!state.user?.jwt,    
    getUser: (state: State) => state.user,
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
export const store = {
  state,
  getters,
  actions,
  mutations
};
