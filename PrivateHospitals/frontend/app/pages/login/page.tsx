'use client';

import React, { useState } from 'react';
import styles from './LoginPage.module.css';
import { useRouter } from 'next/navigation';
import { LoginRequest } from '@/app/services/user';

const LoginPage = () => {
  const router = useRouter();

  const GoToRegisterPatient = () => {
    router.push("/pages/registerPatient");
  };
  
  const GoToRegisterDoctor = () => {
    router.push("/pages/registerDoctor");
  };

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const handleLogin = async (event: React.FormEvent) => {
    event.preventDefault();

    const loginRequest: LoginRequest = {
      Email: email,
      Password: password,
    };

    try {
      const response = await fetch("https://localhost:7262/api/user/LoginUser", {
        method: "POST",
        headers: {
          "content-type": "application/json",
        },
        body: JSON.stringify(loginRequest),
      });

      if (response.ok) {
        const result = await response.json();

        if (result.success) {
          const { email, token } = result.data;
          console.log("Login successful");

          localStorage.setItem("token", token);
          localStorage.setItem("email", email);

          router.push("/");
        } else {
          console.error("Login failed: ", result.errors);
        }
      } else {
        console.error("Login failed with status:", response.status);
      }
    } catch (error) {
      console.error("Error: ", error);
    }
  };

  return (
    <div className={styles.loginContainer}>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <input
          type='email'
          placeholder='Email'
          className={styles.inputField}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type='password'
          placeholder='Password'
          className={styles.inputField}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button type='submit' className={styles.loginButton}>Login</button>
      </form>
      <div className={styles.registerLinks}>
        <p>Register as <button onClick={GoToRegisterPatient}>Patient</button> or <button onClick={GoToRegisterDoctor}>Doctor</button></p>
      </div>
    </div>
  );
};

export default LoginPage;
