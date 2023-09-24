// store.ts
import { InjectionKey } from 'vue'
import { createStore, Store } from 'vuex'
import auth from './modules/auth'
import { User } from '@/Models/User'

// define your typings for the store state
export interface State {
  user: User | null
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  modules: {
    auth
  }
})