// vuex.d.ts
import { Store } from 'vuex'
import { User } from '@/Models/User'

declare module '@vue/runtime-core' {
  // declare your own store states
  interface State {
    user: User | null
  }

  // provide typings for `this.$store`
  interface ComponentCustomProperties {
    $store: Store<State>
  }
}