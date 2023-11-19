import { User } from "@/Models/User";

export function emptyUser() {
    return {
        userName: "",
        name: "",
        role: "User",
        resetPassword: true
    } as User
}