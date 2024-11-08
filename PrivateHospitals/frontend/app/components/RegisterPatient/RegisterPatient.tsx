'use client'

import { PatientRequest } from '@/app/services/patient';
import React, { useState } from 'react'
import styles from './RegisterPage.module.css';
import { patientSchema } from '@/app/validation/patientValidation';
import { useRouter } from 'next/navigation';

const RegisterPatient = () => {
  const [firstName, setFirstName] = useState<string>("");
  const [lastName, setLastName] = useState<string>("");
  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const route = useRouter();

  const handleCreate =  async (event: React.FormEvent) => {
    event.preventDefault(); 
    console.log("Adding started");
    const patientRequest: PatientRequest = {
        FirstName: firstName,
        LastName: lastName,
        Email: email,
        Password: password,
        UserName: userName
    };

    const result = patientSchema.safeParse(patientRequest);

    if(!result.success){
      console.error("Validation Erorr", result.error.errors);
      return;
    }

    try{
        const response = await fetch("https://localhost:7262/api/patient/RegisterPatient", {
            method: "POST",
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify(patientRequest)
        })
        if(response.ok){
            console.log("Pattient was added");
            route.push("/");
        }else{
            console.error("Pattient wasn`t added");
        }
    }catch(error){
        console.error("Error:", error);
    }
  }


  return (
    <div className={styles.registerContainer}>
      <h2>Register Patient</h2>
      <form onSubmit={handleCreate}>
        <input type="text" placeholder="First Name" required className={styles.inputField} value={firstName} onChange={(e) => setFirstName(e.target.value)} />
        <input type="text" placeholder="Last Name" required className={styles.inputField} value={lastName} onChange={(e) => setLastName(e.target.value)} />
        <input type="text" placeholder="Username" required className={styles.inputField} value={userName} onChange={(e) => setUserName(e.target.value)} />
        <input type="email" placeholder="Email" required className={styles.inputField} value={email} onChange={(e) => setEmail(e.target.value)} />
        <input type="password" placeholder="Password" required className={styles.inputField}  value={password} onChange={(e) => setPassword(e.target.value)}/>
        <button className={styles.registerButton}>Register</button>
      </form>
    </div>
  )
}

export default RegisterPatient

