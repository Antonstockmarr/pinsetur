<template>
    <div class="trip-table-content">
        <b-button
            class="mb-2"
            variant="success"
            @click="createTrip"
        >
            Opret Tur
        </b-button>
        <b-table
            striped
            hover
            :busy="loading"
            :items="trips"
            :fields="tripFields"
        >
            <template #cell(actions)="row">
                <b-button variant="primary" size="sm" @click="editTrip(row)" class="m-2">
                    Ret
                </b-button>
                <b-button variant="danger" size="sm" @click="confirmDeleteTrip(row)" class="m-2">
                    X
                </b-button>
            </template>

        </b-table>
        <p class="error-text" v-if="error"> {{ error }}</p>
        <create-trip-form
            v-model:show-form="showCreateTripModal"
            v-on:submitted="$emit('refresh')"
        />
        <edit-trip-form
            v-model:show-form="showEditTripModal"
            :trip="changedTrip"
            v-on:submitted="$emit('refresh')"
        />

        <confirm-modal
            v-model:show-modal="showConfirmDeleteModal"
            :title="'Slet tur for ' + tripToBeDeleted.year"
            :description="`Er du sikker på at du vil slette turen for ${tripToBeDeleted.year}`"
            v-on:confirmed="deleteTrip"
        />

    </div>
</template>


<script lang="ts">
import { PropType, defineComponent } from 'vue';
import { TableItem } from 'bootstrap-vue-next';
import CreateTripForm from './CreateTripForm.vue';
import EditTripForm from './EditTripForm.vue';
import ConfirmModal from '../ConfirmModal.vue';
import { emptyTrip } from '@/common/utils';
import { Trip } from '@/Models/Trip';
import { RowType } from '@/common/types';
import { $api } from '@/common/apiService'

export default defineComponent ({
    name: "TripTable",
    components: {
    CreateTripForm,
    EditTripForm,
    ConfirmModal
},
    props: {
        trips: {
            type: Object as PropType<TableItem[]>,
            required: true
        },
        loadingTrips: {
            type: Boolean,
            required: true
        }
    },
    computed: {
        loading: {
            get(): boolean {
                return this.loadingTrips
            },
            set(value: boolean) {
                this.$emit('update:loadingTrips', value)
            }
        }
    },
    data() {
        return {
            error: "",
            tripFields: [
                {
                    key: 'year',
                    label: 'År'
                },
                {
                    key: 'location',
                    label: 'Sted'
                },
                {
                    key: 'actions',
                    label: 'Handlinger'
                }
            ],
            showCreateTripModal: false,
            showEditTripModal: false,
            changedTrip: emptyTrip(),
            tripToBeDeleted: emptyTrip(),
            showConfirmDeleteModal: false
        }
    },
    methods: {
        createTrip() {
            this.error = ""
            this.showCreateTripModal = true
        },
        editTrip(row: RowType) {
            this.error = ""
            this.changedTrip = row.item as unknown as Trip
            this.showEditTripModal = true
        },
        confirmDeleteTrip(row: RowType) {
            this.tripToBeDeleted = row.item as unknown as Trip
            this.showConfirmDeleteModal = true
        },
        async deleteTrip() {
            this.error = ""
            this.loading = true
            const response = await $api.trips.delete(this.tripToBeDeleted)
            if (response === null) {
                this.error = "Der skete en fejl på serveren. Prøv igen."
                this.loading = false
            }
            else {
                this.$emit('refresh')
            }
        },
    },
});
</script>

<style scoped>
.trip-table-content {
    padding: 15px;
}

.spinner-center {
    display: flex;
    justify-content: center;
}

.error-text {
    color: red;
}

p {
    line-height: 20px;
}
</style>