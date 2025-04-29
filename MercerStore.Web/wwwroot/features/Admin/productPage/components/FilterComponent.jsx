import React from "react";
import {useDispatch} from "react-redux";
import {useProducts} from "../hooks/useProducts";
import {setFilter} from "../redux/adminProductPageSlice";

const FilterComponent = () => {
    const dispatch = useDispatch();
    const {filter} = useProducts();

    const handleFilterChange = (event) => {
        const newFilter = event.target.value;
        dispatch(setFilter(newFilter));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Фильтр:</span>
            <select className="form-select" value={filter} onChange={handleFilterChange}>
                <option value="All">Все</option>
                <option value="InStock">В наличии</option>
                <option value="OutOfStock">Нет в наличии</option>
                <option value="Archived">Снят с продажи</option>
            </select>
        </div>
    );
};

export default FilterComponent;