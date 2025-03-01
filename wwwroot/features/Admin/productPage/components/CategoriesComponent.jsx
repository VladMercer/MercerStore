import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useProducts } from '../hooks/useProducts';
import { setCategoryId, fetchCategories } from '../redux/adminProductPageSlice';

const CategoriesComponent = () => {
    const dispatch = useDispatch();
    const { categories, categoryId } = useProducts();
   
    const handleCategoryChange = (event) => {
        const newCategoryId = event.target.value;
        dispatch(setCategoryId(newCategoryId));
    };

    return (
        <div className="input-group">
            <span className="input-group-text">Категория:</span>
            <select
                className="form-select"
                value={categoryId || ''}
                onChange={handleCategoryChange}
            >
                <option value="">Все категории</option>
                {categories &&
                    categories.map((category) => (
                        <option key={category.id} value={category.id}>
                            {category.name}
                        </option>
                    ))}
            </select>
        </div>
    );
};

export default CategoriesComponent;