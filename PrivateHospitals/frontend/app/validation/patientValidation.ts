import { z } from 'zod';

export const patientSchema = z.object({
    FirstName: z.string().min(1, "First Name is required"),
    LastName: z.string().min(1, "Last Name is required"),
    UserName: z.string().min(1, "User Name is required"),
    Email: z.string().min(1, "First Name is required").email("Email is not valid"),
    Password: z.string().min(6, "Password must be at leasst 6 characters")
})