import React, { useMemo } from "react";
import { useDispatch } from "react-redux";
import { useSuppliers } from "../hooks/useSuppliers";
import useFetchSuppliers from "../hooks/useFetchSuppliers";
import { removeSupplier, fetchSuppliers } from "../redux/supplierPageSlice";

const SuppliersListComponent = () => {
    useFetchSuppliers();
    const dispatch = useDispatch();
    const { suppliers, pageNumber, pageSize, query } = useSuppliers();

    const currentPath = window.location.pathname.toLowerCase();
    const isInvoicePage = currentPath.includes('/invoice');

    const handleSupplierClick = (supplierId) => {
        if (isInvoicePage) {
            window.location.href = `/admin/invoice/create-invoice/${supplierId}`;
        } else {
            window.location.href = `/admin/supplier/update/${supplierId}`;
        }
    };

    const handleDelete = async (supplierId) => {
        if (window.confirm("Вы уверены, что хотите удалить этого поставщика?")) {
            await dispatch(removeSupplier(supplierId)).unwrap();
            dispatch(fetchSuppliers({ pageNumber, pageSize, query }));
        }
    };

    const renderSuppliers = useMemo(() => {
        if (!suppliers || suppliers.length === 0) {
            return (
                <tr>
                    <td colSpan="6" className="text-center">
                        Нет данных
                    </td>
                </tr>
            );
        }

        return suppliers.map((supplier) => (
            <tr
                key={supplier.id}
                style={{ cursor: "pointer", verticalAlign: "middle" }}
                onClick={() => handleSupplierClick(supplier.id)}
            >
                <td className="text-start">
                    <strong>{supplier.name}</strong>
                    <div className="text-muted">
                        {supplier.isCompany ? "Юр. лицо" : "Физ. лицо"}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Контакт:</strong> {supplier.contactPerson || "—"}
                    </div>
                    <div>
                        <strong>Email:</strong> {supplier.email || "—"}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Адрес:</strong> {supplier.address || "—"}
                    </div>
                    <div>
                        <strong>Телефон:</strong> {supplier.phone || "—"}
                    </div>
                </td>

                <td className="text-center">{supplier.taxId || "—"}</td>

                {!isInvoicePage && ( 
                    <td className="text-center">
                        <button
                            className="remove-from-cart-button"
                            onClick={(e) => {
                                e.stopPropagation();
                                handleDelete(supplier.id);
                            }}
                        >
                            <i className="fa-solid fa-trash-can"></i>
                        </button>
                    </td>
                )}
            </tr>
        ));
    }, [suppliers]);

    return (
        <div className="container mt-4">
            <h2>Список поставщиков</h2>
            <table className="table table-striped table-hover table-responsive-md table-sm">
                <thead className="thead-dark">
                    <tr>
                        <th className="text-center">Название</th>
                        <th className="text-center">Контакты</th>
                        <th className="text-center">Адрес и Телефон</th>
                        <th className="text-center">ИНН</th>
                        {!isInvoicePage && (
                            <th className="text-center"></th>
                        )}
                    </tr>
                </thead>
                <tbody>{renderSuppliers}</tbody>
            </table>
        </div>
    );
};

export default SuppliersListComponent;