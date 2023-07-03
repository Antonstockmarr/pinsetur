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
    <b-row>
        <b-col>
           coverImageId 
        </b-col>
        <b-col>
            {{ trip?.coverImageId }}
        </b-col>
    </b-row>

    <div v-if="gallery && gallery.length > 0">
        <h1>Gallery</h1>
        <div v-for="image in gallery" :key="image.id">
            {{ image }}
        </div>
    </div>
    
    
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import { Image } from '@/Models/Image';
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
            trip: null as Trip | null,
            gallery: [] as Image[] | null
        }
    },
    async mounted() {
        this.trip = await $api.trips.get(Number.parseInt(this.id));

        this.gallery = await $api.images.fetch(this.trip?.year);
    }
});
</script>