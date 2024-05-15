<template>
    <div class="pinsetur-modal" :class="{ '--open': show}">
        <div class="modal-content">
            <span class="close" @click="showModal=false">&times;</span>
            <slot></slot>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent ({
    name: "PinseturModal",
    components: {
    },
    props: {
        show: {
            type: Boolean,
            required: true
        }
    },
    emits: ['update:show'],
    computed: {
        showModal: {
            get() {
                return this.show
            },
            set(value: boolean) {
                this.$emit('update:show', value)
            }
        }
    },
    watch: {
        show: function(value) {
            // Prevent scrolling
            console.log(value)
            if (value === true) {
                document.body.style.top = `-${window.scrollY}px`;
                document.body.style.position = 'fixed';
                console.log(window)
                document.body.style.paddingRight = '15px';
            }
            else {
                const scrollY = document.body.style.top;
                document.body.style.paddingRight = '0';
                document.body.style.position = '';
                document.body.style.top = '';
                window.scrollTo(0, parseInt(scrollY || '0') * -1);
            }
        }
    }
});
</script>

<style scoped>

.pinsetur-modal {
  display: none; /* Hidden by default */
  position: fixed; /* Stay in place */
  z-index: 100; /* Sit on top */
  left: 0;
  top: 0;
  width: 100%; /* Full width */
  height: 100%; /* Full height */
  background-color: rgb(0,0,0); /* Fallback color */
  background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
}

.pinsetur-modal.--open {
    display: block;
}

/* Modal Content/Box */
.modal-content {
  background-color: #fefefe;
  margin: 5vh auto; /* 15% from the top and centered */
  padding: 20px;
  border: 1px solid #888;
  overflow: hidden;
  width: 90vw; /* Could be more or less, depending on screen size */
  height: 90vh;
}

@media only screen and (max-width: 600px) {
    .modal-content {
        width: 100vw;
        height: 100vh;
        margin: 0;
    }
}

/* The Close Button */
.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
</style>