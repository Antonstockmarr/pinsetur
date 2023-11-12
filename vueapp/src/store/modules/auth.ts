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
    logOut({ commit }: {commit: Function}) {
        commit('logOut')
    },
    setSession({ commit }: {commit: Function}, session: Session) {
        commit('setSession', session)
    }
};
const mutations = {
    setSession(state: State, session: Session){
        state.session = session
    },
    logOut(state: State){
        state.session = {
            jwt: "",
            user: {
                userName: "",
                name: "",
                role: "",
                resetPassword: true
            }},
        state.token = null
    
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
