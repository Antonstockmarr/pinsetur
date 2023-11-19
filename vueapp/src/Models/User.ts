export interface User {
    userName: string
    name: string
    role: "Admin" | "User"
    resetPassword: boolean
}