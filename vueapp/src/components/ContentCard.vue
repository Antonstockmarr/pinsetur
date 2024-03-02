<template>
    <b-card
        :style="style"
    >
        <slot></slot>
    </b-card>    
</template>


<script lang="ts">
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "ContentCard",
    computed: {
        style() {
            if (this.$store.getters.windowWidthLessThan1000px) {
                return `
                `
            }
            else return `
                border: 2px black solid;
                box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
            `
        }
    }
    });
</script>
