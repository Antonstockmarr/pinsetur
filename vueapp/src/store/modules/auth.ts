import { Session } from '@/Models/Session';
import { User } from '@/Models/User';

export type State = {
    session: Session | null
    token: string | null
}
const state: State = {
    session: {
        jwt: "",
        user: {
            userName: "",
            name: "",
            role: "",
            resetPassword: true
        }},
    token: null
};
const getters = {
    isAuthenticated: (state: State) => !!state.session?.jwt,    
    getSession: (state: State) => state.session,
};
const actions = {
};
const mutations = {
    setSession(state: State, session: Session){
        state.session = session
    },
    LogOut(state: State){
        state.session = null
    },
    updateUser(state: State, user: User) {
        if (!state.session) {
            state.session = { jwt: "", user: user}
        }
        else {
            state.session.user = user
        }
    }
};
export const store = {
  state,
  getters,
  actions,
  mutations
};
