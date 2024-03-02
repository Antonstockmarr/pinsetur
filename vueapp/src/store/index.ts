// store.ts
import { InjectionKey } from 'vue'
import { createStore, Store } from 'vuex'
import { store as auth, State as authState} from './modules/auth'
import { store as browser, State as BrowserState} from './modules/browser'

// define your typings for the store state
export interface State {
  browser: BrowserState
  auth: authState
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  modules: {
    auth,
    browser
  }
})