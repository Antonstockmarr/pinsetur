<template>
    <div class="user-table-content">
        <b-button
            class="mb-2"
            variant="success"
            @click="createUser"
        >
            Opret Bruger
        </b-button>
        <b-table
            striped
            hover
            :busy="loading"
            :items="users"
            :fields="userFields"
        >
            <template #cell(actions)="row">
                <b-button variant="primary" size="sm" @click="editUser(row)" class="m-2">
                    Ret
                </b-button>
                <b-button size="sm" @click="resetPassword(row)" class="m-2">
                    Nulstil kodeord
                </b-button>
                <b-button variant="danger" size="sm" @click="deleteUser(row)" class="m-2">
                    X
                </b-button>
            </template>
        </b-table>
        <b-modal
            v-model:model-value="showNewPasswordModal"
            title="Nyt kodeord"
        >
            <p>Brugeren <strong>{{ changedUser?.userName }}</strong> har fået nulstillet sit password. Det midlertidige password er:</p>
            <p>
                <strong>{{ newPassword }}</strong>
            </p>
            <p>Ved login vil brugeren blive bedt om at sætte et nyt password.</p>
        </b-modal>
        <edit-user-form
            v-model:show-form="showEditUserForm"
            :user="changedUser"
            v-on:submitted="$emit('refresh')"
        />
        <p class="error-text" v-if="error"> {{ error }}</p>
    </div>
</template>


<script lang="ts">
import { User } from '@/Models/User';
import { PropType, defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import type { RowType } from '@/common/types.ts'
import { TableItem } from 'bootstrap-vue-next';
import EditUserForm from './EditUserForm.vue';
import { emptyUser } from '@/common/utils';

export default defineComponent ({
    name: "UserTable",
    components: {
    EditUserForm
},
    props: {
        users: {
            type: Object as PropType<TableItem[]>,
            required: true
        },
        loadingUsers: {
            type: Boolean,
            required: true
        }
    },
    computed: {
        loading: {
            get(): boolean {
                return this.loadingUsers
            },
            set(value: boolean) {
                this.$emit('update:loadingUsers', value)
            }
        }
    },
    data() {
        return {
            error: "",
            showNewPasswordModal: false,
            newPassword: "",
            showEditUserForm: false,
            changedUser: emptyUser() as User,
            userFields: [
                {
                    key: 'userName',
                    label: 'Brugernavn'
                },
                {
                    key: 'name',
                    label: 'Navn'
                },
                {
                    key: 'role',
                    label: 'Rolle'
                },
                {
                    key: 'resetPassword',
                    label: 'Anmod om nyt kodeord'
                },
                {
                    key: 'actions',
                    label: 'Handlinger'
                }
            ]
        }
    },
    methods: {
        async resetPassword(row: RowType) {
            this.error = ""
            this.loading = true
            this.changedUser = row.item as unknown as User
            const response = await $api.users.resetPassword(this.changedUser.userName)
            if (response === null) {
                this.error = "Der skete en fejl på serveren. Prøv igen."
            }
            else {
                this.newPassword = response
                this.$emit('refresh')
                this.showNewPasswordModal = true
            }
            this.loading = false
        },
        editUser(row: RowType) {
            this.changedUser = row.item as unknown as User
            this.showEditUserForm = true
        },
        async deleteUser(row: RowType) {
            this.error = "Denne funktion virker ikke endnu"
        },
        async createUser() {
            this.error = "Denne funktion virker ikke endnu"
        },
        refresh() {

        }
    },
});

</script>

<style scoped>
.user-table-content {
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