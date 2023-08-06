<template>
    <b-card
        :style="'border: 2px black solid; box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);'"
    >
        <slot></slot>
    </b-card>    
</template>


<script lang="ts">
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "ContentCard"
    });
</script>
