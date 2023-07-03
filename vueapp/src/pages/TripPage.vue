<template>
    <b-button @click="$router.back()">
        Tilbage
    </b-button>
    <b-row>
        <b-col>
           Year 
        </b-col>
        <b-col>
            {{ trip?.year }}
        </b-col>
    </b-row>
    <b-row>
        <b-col>
           Location 
        </b-col>
        <b-col>
            {{ trip?.location }}
        </b-col>
    </b-row>
    <b-row>
        <b-col>
           Address 
        </b-col>
        <b-col>
            {{ trip?.address }}
        </b-col>
    </b-row>
    <b-row>
        <b-col>
           Description 
        </b-col>
        <b-col>
            {{ trip?.description }}
        </b-col>
    </b-row>
    <b-row>
        <b-col>
           locationImageId 
        </b-col>
        <b-col>
            {{ trip?.locationImageId }}
        </b-col>
    </b-row>
    
    
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import { $api } from '@/common/apiService';
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "TripPage",
    props: {
        id: {
            type: String,
            required: true
        }
    },
    data () {
        return {
            trip: null as Trip | null
        }
    },
    async mounted() {
        this.trip = await $api.trips.get(Number.parseInt(this.id));
    }
});
</script>