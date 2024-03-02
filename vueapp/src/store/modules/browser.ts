export type State = {
    windowWidthLessThan1000px: boolean
}
const state: State = {
    windowWidthLessThan1000px: false
};
const getters = {
    windowWidthLessThan1000px: (state: State) => state.windowWidthLessThan1000px
};
const mutations = {
    setWindowWidthLessThan1000px(state: State, windowWidthLessThan1000px: boolean){
        state.windowWidthLessThan1000px = windowWidthLessThan1000px
    },
};
export const store = {
  state,
  getters,
  mutations
};
