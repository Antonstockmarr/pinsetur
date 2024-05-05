export type State = {
    windowWidthLessThan1000px: boolean
    windowWidthLessThan600px: boolean
}
const state: State = {
    windowWidthLessThan1000px: false,
    windowWidthLessThan600px: false
};
const getters = {
    windowWidthLessThan1000px: (state: State) => state.windowWidthLessThan1000px,
    windowWidthLessThan600px: (state: State) => state.windowWidthLessThan600px
};
const mutations = {
    setWindowWidthLessThan1000px(state: State, windowWidthLessThan1000px: boolean){
        state.windowWidthLessThan1000px = windowWidthLessThan1000px
    },
    setWindowWidthLessThan600px(state: State, windowWidthLessThan600px: boolean){
        state.windowWidthLessThan600px = windowWidthLessThan600px
    },
};
export const store = {
  state,
  getters,
  mutations
};
