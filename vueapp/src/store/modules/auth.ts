import { Session } from '@/Models/Session';
import { User } from '@/Models/User';
import { emptyUser } from '@/common/utils';

export type State = {
    session: Session | null
    sasToken: string | null
}
const state: State = {
    session: {
        jwt: "",
        user: emptyUser()},
    sasToken: null
};
const getters = {
    isAuthenticated: (state: State) => !!state.session?.jwt,
    isAdmin: (state: State) => state.session?.user.role === "Admin",    
    getSession: (state: State) => state.session,
    getSasToken: (state: State) => state.sasToken
};
const actions = {
    logOut({ commit }: {commit: Function}) {
        commit('logOut')
    },
    setSession({ commit }: {commit: Function}, session: Session) {
        commit('setSession', session)
    },
    setSasToken({ commit }: {commit: Function}, sasToken: string) {
        commit('setSasToken', sasToken)
    },
    updateUser({ commit }: {commit: Function}, user: User) {
        commit('updateUser', user)
    }
};
const mutations = {
    setSession(state: State, session: Session){
        state.session = session
    },
    setSasToken(state: State, sasToken: string) {
        state.sasToken = sasToken
    },
    logOut(state: State){
        state.session = {
            jwt: "",
            user: emptyUser()},
        state.sasToken = null
    
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
