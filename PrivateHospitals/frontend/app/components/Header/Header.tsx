'use client'

import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';

const Header = () => {
  const router = useRouter();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    setIsAuthenticated(!!token);
  }, []);

  const handleLoginClick = () => {
    router.push("/pages/login");
  }

  const handleLogout = () => {
    // Видаляємо токен з localStorage
    localStorage.removeItem("token");
    localStorage.removeItem("email");
    setIsAuthenticated(false); // Оновлюємо стан
    router.push("/"); // Повертаємося на головну сторінку
  };

  return (
    <header className="header">
      <div className="header__container">
        <div className="header__logo">
          <h1>Hospital</h1>
        </div>
        <nav className="header__nav">
          {isAuthenticated ? (
          <button className="header__login-button" onClick={handleLogout}>Вийти</button>
        ) : (
          <button className="header__login-button" onClick={handleLoginClick}>Увійти</button>
        )}
        </nav>
      </div>
    </header>
  );
};

export default Header;
