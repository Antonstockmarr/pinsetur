<template>
  <div v-if="isIsoDateString(startDate) && isIsoDateString(endDate)" class="dates">
    <div class="date">
      <p>{{ getEndDate() }}</p>
      <span>{{ getEndMonth() }}</span>  
    </div>
    <i class="right-arrow"></i>
    <div class="date">
      <p>{{ getStartDate() }}</p>
      <span>{{ getStartMonth() }}</span>
    </div>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
const isoDateFormat = /^\d{4}-\d{2}-\d{2}/;

const danishMonths = ['Januar', 'Februar', 'Marts', 'April', 'Maj', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'December']

export default defineComponent({
  name: 'CalendarCard',
  props: {
    startDate: {
      type: String,
      required: true
    },
    endDate: {
      type: String,
      required: true
    }
  },
  methods: {
    isIsoDateString(value: any): boolean {
      return value && typeof value === "string" && isoDateFormat.test(value);
    },
    getStartDate() {
      if (this.isIsoDateString(this.startDate)) {
        const date = new Date(this.startDate)
        return date.getDate()
      }
    },
    getStartMonth() {
      if (this.isIsoDateString(this.startDate)) {
        const date = new Date(this.startDate)
        return danishMonths[date.getMonth()]
      }
    },
    getEndDate() {
      if (this.isIsoDateString(this.endDate)) {
        const date = new Date(this.endDate)
        return date.getDate()
      }
    },
    getEndMonth() {
      if (this.isIsoDateString(this.endDate)) {
        const date = new Date(this.endDate)
        return danishMonths[date.getMonth()]
      }
    }
  }
})


</script>


<style scoped>

.date {
  font-size: 14px;
  border-radius: 10px;
  height: 65px;
  width: 65px;
  text-align: center;
  position: relative;
  overflow: hidden;
  background-color: var(--col2);
  border: 1px solid black;
  box-shadow: 0 0 10px, rgba(0,0,0,0.1);
  float: right;

  & p {
    font-size: 24px;
    line-height: 40px;
    font-weight: 800;
  }
}


.date span {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  background: #dd3936;
  font-size: 11px;
  font-weight: 700;
  padding: 4px 0;
}

.right-arrow {
  margin: 20px;
  border: solid black;
  border-width: 0 10px 10px 0;
  float: right;
  padding: 3px;
  transform: rotate(-45deg);
  -webkit-transform: rotate(-45deg);
}

</style>