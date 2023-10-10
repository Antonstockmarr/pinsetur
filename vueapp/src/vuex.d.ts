// vuex.d.ts
import { Store } from 'vuex'
import { User } from '@/Models/User'
import { State } from '@/store'

declare module '@vue/runtime-core' {
  // declare your own store states

  // provide typings for `this.$store`
  interface ComponentCustomProperties {
    $store: Store<State>
  }
}