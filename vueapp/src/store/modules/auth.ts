import { Session } from '@/Models/Session';
import { User } from '@/Models/User';
import { emptyUser } from '@/common/utils';

export type State = {
    session: Session | null
    token: string | null
}
const state: State = {
    session: {
        jwt: "",
        user: emptyUser()},
    token: null
};
const getters = {
    isAuthenticated: (state: State) => !!state.session?.jwt,
    isAdmin: (state: State) => state.session?.user.role === "Admin",    
    getSession: (state: State) => state.session,
};
const actions = {
    logOut({ commit }: {commit: Function}) {
        commit('logOut')
    },
    setSession({ commit }: {commit: Function}, session: Session) {
        commit('setSession', session)
    },
    updateUser({ commit }: {commit: Function}, user: User) {
        commit('updateUser', user)
    }
};
const mutations = {
    setSession(state: State, session: Session){
        state.session = session
    },
    logOut(state: State){
        state.session = {
            jwt: "",
            user: emptyUser()},
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
