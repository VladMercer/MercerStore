import React from 'react';

const SearchBarComponent = ({ query, setQuery, results, isDropdownVisible, setDropdownVisible, handleSubmit }) => {

    return (
        <div className="position-relative">
            <form onSubmit={handleSubmit} className="d-flex input-group">
                <input
                    type="text"
                    className="form-control"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder="Поиск"
                    aria-label="Search"
                    aria-describedby="button-search"
                    onFocus={() => setDropdownVisible(true)}
                />
                <button className="btn btn-outline-warning" type="submit" id="button-search">
                    <i className="fa-solid fa-magnifying-glass"></i>
                </button>
            </form>
            {isDropdownVisible && results.length > 0 &&(
                <div className="search-dropdown position-absolute bg-white border rounded shadow-sm w-100">
                    {results.map(product => (
                        <div
                            key={product.id}
                            className="search-result-item d-flex align-items-center p-2"
                            onClick={() => window.location.href = `/product/details/${product.id}`}
                            style={{ cursor: 'pointer' }}
                        >
                            <img src={product.mainImageUrl} alt={product.name} />
                            <div>
                                <h5 dangerouslySetInnerHTML={{ __html: product.name }}></h5>
                                <p>{product.description}</p>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default SearchBarComponent;