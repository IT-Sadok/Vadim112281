import { z } from "zod"

export const doctorSchema = z.object({
    FirstName: z.string().min(1, "First Name is required"),
    LastName: z.string().min(1, "Last Name is required"),
    UserName: z.string().min(1, "User Name is required"),
    Email: z.string().min(1, "First Name is required").email("Email is invalid"),
    Speciality: z.string().min(1, "Speciality is required"),
    Password: z.string().min(1, "First Name is required")
});