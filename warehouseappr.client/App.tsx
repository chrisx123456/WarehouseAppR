import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navigation from './components/Navigation/Navigation';
import Content from './components/Content/Content';
import Home from './pages/Home';
import Users from './pages/Users';
import AddUser from './pages/AddUser';
import Products from './pages/Products';
import AddProduct from './pages/AddProduct';
import AdminPanel from './pages/AdminPanel'
import { User, UserRole } from './types/User';

const App: React.FC = () => {
    const [user, setUser] = useState<User>({ role: UserRole.Admin }); // Tutaj ustaw rolę użytkownika
    return (
        <Router>
            <div style={{ display: 'flex' }}>
                <Navigation user={user} />
                <Content>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/users" element={<Users />} />
                        <Route path="/users/add" element={<AddUser />} />
                        <Route path="/products" element={<Products />} />
                        <Route path="/products/add" element={<AddProduct />} />
                        <Route path="/admin" element={<AdminPanel />} />
                    </Routes>
                </Content>
            </div>
        </Router>
    );
};

export default App;