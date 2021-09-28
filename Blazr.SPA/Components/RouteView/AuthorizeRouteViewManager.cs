// ==========================================================
//  Original code:
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// ============================================================

/// =================================
/// Mods Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

#nullable disable warnings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

namespace Blazr.SPA.Components
{
    public class AuthorizeRouteViewManager : RouteViewManager
    {
        private static readonly RenderFragment<AuthenticationState> _defaultNotAuthorizedContent
            = state => builder => builder.AddContent(0, "Not authorized");
        private static readonly RenderFragment _defaultAuthorizingContent
            = builder => builder.AddContent(0, "Authorizing...");

        private readonly RenderFragment _renderAuthorizeRouteViewCoreDelegate;
        private readonly RenderFragment<AuthenticationState> _renderAuthorizedDelegate;
        private readonly RenderFragment<AuthenticationState> _renderNotAuthorizedDelegate;
        private readonly RenderFragment _renderAuthorizingDelegate;

        [Parameter] public RenderFragment<AuthenticationState>? NotAuthorized { get; set; }

        [Parameter] public RenderFragment? Authorizing { get; set; }

        [Parameter] public object Resource { get; set; }

        [CascadingParameter] private Task<AuthenticationState> ExistingCascadedAuthenticationState { get; set; }

        public AuthorizeRouteViewManager()
        {
            RenderFragment renderBaseRouteViewDelegate = builder => base.RenderRouteView(builder);
            _renderAuthorizedDelegate = authenticateState => renderBaseRouteViewDelegate;
            _renderNotAuthorizedDelegate = authenticationState => builder => RenderNotAuthorizedInDefaultLayout(builder, authenticationState);
            _renderAuthorizingDelegate = RenderAuthorizingInDefaultLayout;
            _renderAuthorizeRouteViewCoreDelegate = RenderAuthorizeRouteViewCore;
        }

        protected override RenderFragment RenderRouteView => builder =>
        {
            RenderEventQueued = false;
            if (ExistingCascadedAuthenticationState != null)
            {
                _renderAuthorizeRouteViewCoreDelegate(builder);
            }
            else
            {
                builder.OpenComponent<CascadingAuthenticationState>(0);
                builder.AddAttribute(1, nameof(CascadingAuthenticationState.ChildContent), _renderAuthorizeRouteViewCoreDelegate);
                builder.CloseComponent();
            }
        };


        private void RenderNotAuthorizedInDefaultLayout(RenderTreeBuilder builder, AuthenticationState authenticationState)
        {
            var content = NotAuthorized ?? _defaultNotAuthorizedContent;
            RenderContentInDefaultLayout(builder, content(authenticationState));
        }

        private void RenderAuthorizingInDefaultLayout(RenderTreeBuilder builder)
        {
            var content = Authorizing ?? _defaultAuthorizingContent;
            RenderContentInDefaultLayout(builder, content);
        }

        private void RenderContentInDefaultLayout(RenderTreeBuilder builder, RenderFragment content)
        {
            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, nameof(LayoutView.Layout), DefaultLayout);
            builder.AddAttribute(2, nameof(LayoutView.ChildContent), content);
            builder.CloseComponent();
        }

        private void RenderAuthorizeRouteViewCore(RenderTreeBuilder builder)
        {
            builder.OpenComponent<AuthorizeRouteViewCore>(0);
            builder.AddAttribute(1, nameof(AuthorizeRouteViewCore.RouteData), RouteData);
            builder.AddAttribute(2, nameof(AuthorizeRouteViewCore.Authorized), _renderAuthorizedDelegate);
            builder.AddAttribute(3, nameof(AuthorizeRouteViewCore.Authorizing), _renderAuthorizingDelegate);
            builder.AddAttribute(4, nameof(AuthorizeRouteViewCore.NotAuthorized), _renderNotAuthorizedDelegate);
            builder.AddAttribute(5, nameof(AuthorizeRouteViewCore.Resource), Resource);
            builder.CloseComponent();
        }

        private sealed class AuthorizeRouteViewCore : AuthorizeViewCore
        {
            [Parameter]
            public RouteData RouteData { get; set; } = default!;

            protected override IAuthorizeData[]? GetAuthorizeData()
                => AttributeAuthorizeDataCache.GetAuthorizeDataForType(RouteData.PageType);
        }
    }
}
