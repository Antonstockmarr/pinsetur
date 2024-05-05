<template>
    <template v-if="divMode">
        <div :style="style">
            <slot></slot>
        </div>
    </template>
    <template v-else>
        <b-card :style="style">
        <slot></slot>
    </b-card>    

    </template>
</template>


<script lang="ts">
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "ContentCard",
    computed: {
        divMode() {
            return this.$store.getters.windowWidthLessThan600px
        },
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
