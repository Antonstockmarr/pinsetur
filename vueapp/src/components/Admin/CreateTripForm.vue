<template>
    <b-offcanvas v-model="show" :title="`Opret Tur`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <b-form @submit="createTrip">
                <b-form-group label="År">
                    <b-form-select v-model="newTrip.year"  :options="yearOptions"></b-form-select>
                </b-form-group>

                <b-form-group label="Lokation">
                    <b-form-input
                        v-model="newTrip.location"
                        text="text"
                    >
                    </b-form-input>
                </b-form-group>

                <b-form-group label="Adresse">
                    <b-form-input
                        v-model="newTrip.address"
                        text="text"
                    >
                    </b-form-input>

                </b-form-group>
                <b-form-group label="Beskrivelse">
                    <b-form-textarea
                        v-model="newTrip.description"
                    >
                    </b-form-textarea>
                </b-form-group>

                <p v-if="error" class="text-danger">{{ error }}</p>
                <b-button class="submit-button" size="lg" type="submit" variant="success">Opret</b-button>
            </b-form>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import { newTrip } from '@/common/utils';
import { NewTrip } from '@/Models/NewTrip';


export default defineComponent ({
    name: "CreateTripForm",
    components: {
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        }
    },
    emits: ['update:showForm', 'submitted'],
    computed: {
        show: {
            get() {
                return this.showForm
            },
            set(value: boolean) {
                this.$emit('update:showForm', value)
            }
        }
    },
    data() {
        return {
            newTrip: newTrip() as NewTrip,
            loading: false as Boolean,
            error: "" as String,
            yearOptions: [] as number[]
        }
    },
    watch: {
        showForm: function() {
            this.error = ""
            this.newTrip = newTrip()
        }
    },
    mounted() {
        this.newTrip = newTrip()
        const maxYear = new Date().getFullYear() + 5
        const startYear = 2000
        this.yearOptions = Array.from({length: maxYear - startYear}, (value, index) => startYear + index)
    },
    methods: {
        async createTrip() {
            this.error = ""
            this.loading = true
            const trip = await $api.trips.create(this.newTrip)
            this.loading = false
            if (trip) {
                this.show = false
                this.$emit('submitted')            }
            else {
                this.error = "Der opstod en fejl. Prøv igen"
            }
        }
    }
});
</script>

<style scoped>
.submit-button {
    position: absolute;
    bottom: 30px;
    right: 30px;
    left: 30px;
}

p {
    line-height: 20px;
}

</style>