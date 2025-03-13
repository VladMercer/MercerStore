import React from "react";
import { useDispatch } from "react-redux";
import { useUsers } from "../hooks/useUsers";
import { setSortOrder } from "../redux/userPageSlice";

const SortComponent = () => {
    const dispatch = useDispatch();
    const { sortOrder } = useUsers();

    const handleSortChange = (event) => {
        dispatch(setSortOrder(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировка:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="NameAsc">Имя (А-Я)</option>
                <option value="NameDesc">Имя (Я-А)</option>
                <option value="CreateDateAsc">Сначала старые</option>
                <option value="CreateDateDesc">Сначала новые</option>
                <option value="LastActivityDateAsc">Были недавно</option>
                <option value="LastActivityDateDesc">Давно не заходили</option>
                <option value="Online">Сначала онлайн</option>
                <option value="Ofline">Сначала оффлайн</option>
            </select>
        </div>
    );
};

export default SortComponent;